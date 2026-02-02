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
	if (depth <= 0)
		return color(0, 0, 0);

	hit_record rec;
	if (!w.scene.hit(r, interval(0.001, infinity), rec))
		return w.background;

	color Le = rec.mat->emitted(r, rec, rec.u, rec.v, rec.p);
	scatter_record srec;

	if (rec.mat->scatter(r, rec, srec) && srec.skip_pdf)
		return Le + srec.attenuation * cast_ray(srec.skip_pdf_ray, depth - 1, w);

	color L_direct(0, 0, 0);
	color diffuse_brdf = srec.attenuation * (1.0 / pi);

	for (size_t i = 0; i < w.lights.objects.size(); ++i) {

		vec3 to_light = w.lights.objects[i]->random(rec.p);
		double dist = to_light.length();
		if (dist <= 0.0) continue;

		vec3 wi = unit_vector(to_light);

		ray shadow_ray(rec.p, wi);
		color Le_light(0, 0, 0);
		hit_record lrec;

		w.scene.hit(shadow_ray, interval(EPSILON, dist + EPSILON), lrec);

		if(lrec.mat)
			Le_light = lrec.mat->emitted(shadow_ray, lrec, lrec.u, lrec.v, lrec.p);

		double n_dot_l = std::max(0.0, dot(rec.normal, wi));
		L_direct += diffuse_brdf * Le_light * n_dot_l / sqrt(dist);
	}

	return Le + L_direct;
}
