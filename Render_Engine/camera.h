#pragma once

#include "vec3.h"
#include "ray.h"
#include "color.h"
#include "shape.h"
#include "material.h"

#include <iostream>

class camera {
public:
	double aspect_ratio = 1.0;      // Ratio of image width over height
	int    image_width = 100;       // Rendered image width in pixel count
	int    samples_per_pixel = 1;   // Count of random samples for each pixel
	int    max_depth = 10;          // Maximum number of ray bounces into scene
	color  background;              // Scene background color

	double vfov = 90;                   // Vertical view angle (field of view)
	point3 lookfrom = point3(0, 0, 0);  // Point camera is looking from
	point3 lookat = point3(0, 0, -1);   // Point camera is looking at
	vec3   vup = vec3(0, 1, 0);         // Camera-relative "up" direction

	double defocus_angle = 0;  // Variation angle of rays through each pixel
	double focus_dist = 10;    // Distance from camera lookfrom point to plane of perfect focus

	void render(const shape& world);

private:
	int    image_height;        // Rendered image height
	double pixel_samples_scale; // Color scale factor for a sum of pixel samples
	point3 center;              // Camera center
	point3 pixel00_loc;         // Location of pixel 0, 0
	vec3   pixel_delta_u;       // Offset to pixel to the right
	vec3   pixel_delta_v;       // Offset to pixel below
	vec3   u, v, w;             // Camera frame basis vectors

	vec3   defocus_disk_u;       // Defocus disk horizontal radius
	vec3   defocus_disk_v;       // Defocus disk vertical radius

	void initialize();
	vec3 sample_square() const;
	ray get_ray(int i, int j) const;
	point3 defocus_disk_sample() const;
	color cast_ray(const ray& r, int depth, const shape& world);


	//color cast_ray_whitted(const ray& r, int depth, const shape& world) {
	//    if (depth <= 0)
	//        return color(0, 0, 0);

	//    hit_record rec;
	//    if (!world.hit(r, interval(0.001, infinity), rec))
	//        return background;

	//    // Always add emission
	//    color result = rec.mat->emitted(rec.u, rec.v, rec.p);

	//    // -------------------------
	//    // Lambertian → direct light
	//    // -------------------------
	//    if (auto lam = dynamic_cast<const lambertian*>(rec.mat.get())) {

	//        for (const auto& light : world.lights) {
	//            vec3 to_light = light.position - rec.p;
	//            double dist = to_light.length();
	//            vec3 L = unit_vector(to_light);

	//            // Shadow ray
	//            ray shadow(rec.p + 0.001 * L, L);
	//            hit_record shadow_hit;

	//            if (world.hit(shadow, interval(0.001, dist), shadow_hit))
	//                continue;

	//            double n_dot_l = dot(rec.normal, L);
	//            if (n_dot_l > 0) {
	//                result += lam->tex->value(rec.u, rec.v, rec.p)
	//                    * light.intensity
	//                    * n_dot_l;
	//            }
	//        }
	//    }

	//    // -------------------------
	//    // Metal → perfect reflection
	//    // -------------------------
	//    else if (auto met = dynamic_cast<const metal*>(rec.mat.get())) {

	//        vec3 reflected = reflect(unit_vector(r.direction()), rec.normal);
	//        ray reflected_ray(rec.p + 0.001 * reflected, reflected);

	//        color reflected_color =
	//            cast_ray_whitted(reflected_ray, depth - 1, world);

	//        result += met->albedo * reflected_color;
	//    }

	//    // -------------------------
	//    // Dielectric → refraction
	//    // -------------------------
	//    else if (auto die = dynamic_cast<const dielectric*>(rec.mat.get())) {

	//        vec3 dir = unit_vector(r.direction());
	//        double eta = rec.front_face
	//            ? (1.0 / die->refraction_index)
	//            : die->refraction_index;

	//        vec3 refracted;
	//        vec3 reflected = reflect(dir, rec.normal);

	//        double cos_theta = fmin(dot(-dir, rec.normal), 1.0);
	//        double reflect_prob =
	//            dielectric::reflectance(cos_theta, eta);

	//        if (random_double() < reflect_prob) {
	//            ray refl_ray(rec.p + 0.001 * reflected, reflected);
	//            result += cast_ray_whitted(refl_ray, depth - 1, world);
	//        }
	//        else if (refract(dir, rec.normal, eta, refracted)) {
	//            ray refr_ray(rec.p + 0.001 * refracted, refracted);
	//            result += cast_ray_whitted(refr_ray, depth - 1, world);
	//        }
	//    }

	//    return result;
	//}

};
