#include "tracer.h"
#include "color.h"
#include "shape.h"
#include "world.h"

color path_tracing::cast_ray(const ray& r, int depth, const world& w) const {

	if (depth <= 0)
		return color(0, 0, 0);

	hit_record rec;
	// If the ray hits nothing, return the background color.
	if (!w.scene.hit(r, interval(0.001, infinity), rec))
		return w.background;

	scatter_record srec;
	color color_from_emission = rec.mat->emitted(r, rec, rec.u, rec.v, rec.p);

	if (!rec.mat->scatter(r, rec, srec))
		return color_from_emission;
	if (srec.skip_pdf)
		return srec.attenuation * cast_ray(srec.skip_pdf_ray, depth - 1, w);

	auto light_ptr = make_shared<shape_pdf>(w.lights, rec.p);
	mixture_pdf p(light_ptr, srec.pdf_ptr);
	ray scattered = ray(rec.p, p.generate());

	auto pdf_value = p.value(scattered.direction());
	double scattering_pdf = rec.mat->scattering_pdf(r, rec, scattered);

	color color_from_scatter =
		(srec.attenuation * scattering_pdf * cast_ray(scattered, depth - 1, w)) / pdf_value;

	return color_from_emission + color_from_scatter;
}

color ray_tracing::cast_ray(const ray& r, int depth, const world& w) const {
	// Base case: stop recursion
	if (depth <= 0)
		return color(0, 0, 0);

	hit_record rec;
	// Ray misses the scene
	if (!w.scene.hit(r, interval(0.001, infinity), rec))
		return w.background;

	// Emission from the material (if any)
	color emitted = rec.mat->emitted(r, rec, rec.u, rec.v, rec.p);

	scatter_record srec;
	// Material does not scatter: just emit
	if (!rec.mat->scatter(r, rec, srec))
		return emitted;

	color scattered_color(0, 0, 0);

	if (!srec.skip_pdf) {
		for (int i = 0; i < w.lights.objects.size(); ++i) {
			
			// For point light illumination
			//vec3 to_light = w.lights.objects[i]->get_center() - rec.p;

			// Sample a random point on the light
			vec3 to_light = w.lights.objects[i]->random(rec.p);
			
			// Store distance to light squared for interval
			double distance_squared = to_light.length_squared();
			vec3 light_dir = unit_vector(to_light);

			// Create shadow ray
			//ray shadow_ray(rec.p, light_dir);
			ray shadow_ray(rec.p, light_dir);
			hit_record temp_rec;
			scatter_record temp_srec;

			// If something blocks the light and is not a light, skip this light
			if (w.scene.hit(shadow_ray, interval(EPSILON, sqrt(distance_squared)), temp_rec)
				&& temp_rec.mat->scatter(shadow_ray, temp_rec, temp_srec))
					continue;

			// Lambert cosine law
			double n_dot_l = fmax(dot(rec.normal, light_dir), 0.0);
			scattered_color += srec.attenuation * n_dot_l;
		}
	}

	// Recursively trace scattered ray
	color recursive_color = cast_ray(srec.skip_pdf_ray, depth - 1, w);

	// Combine emission, direct lighting, and recursive contribution
	return emitted + scattered_color + srec.attenuation * recursive_color;
}