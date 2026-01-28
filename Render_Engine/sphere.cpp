#include "sphere.h"

#include <cmath>

// Constructor
sphere::sphere(const point3& center, double radius, shared_ptr<material> mat) : center(center), radius(std::fmax(0, radius)), mat(mat) {
	auto rvec = vec3(radius, radius, radius);
	bbox = aabb(center - rvec, center + rvec);
}

aabb sphere::bounding_box() const {
	return bbox;
}

bool sphere::hit(const ray& r, interval ray_t, hit_record& rec) const {
	vec3 oc = center - r.origin();

	auto a = r.direction().length_squared();
	auto h = dot(r.direction(), oc);
	auto c = oc.length_squared() - radius * radius;

	auto discriminant = h * h - a * c;
	if (discriminant < 0)
		return false;

	auto sqrtd = std::sqrt(discriminant);

	// Find the nearest root in the acceptable range
	auto root = (h - sqrtd) / a;
	if (!ray_t.surrounds(root)) {
		root = (h + sqrtd) / a;
		if (!ray_t.surrounds(root))
			return false;
	}

	rec.t = root;
	rec.p = r.at(rec.t);
	vec3 outward_normal = (rec.p - center) / radius;
	rec.set_face_normal(r, outward_normal);
	get_sphere_uv(outward_normal, rec.u, rec.v);
	rec.mat = mat;

	return true;
}



void sphere::get_sphere_uv(const point3& p, double& u, double& v) {
	// p: a given point on the sphere of radius one, centered at the origin.
	// u: returned value [0,1] of angle around the Y axis from X=-1.
	// v: returned value [0,1] of angle from Y=-1 to Y=+1.

	auto theta = std::acos(-p.y());
	auto phi = std::atan2(-p.z(), p.x()) + pi;

	u = phi / (2 * pi);
	v = theta / pi;
}

double sphere::pdf_value(const point3& origin, const vec3& direction) const {
	// This method only works for stationary spheres.

	hit_record rec;
	if (!this->hit(ray(origin, direction), interval(0.001, infinity), rec))
		return 0;

	auto dist_squared = (center - origin).length_squared();
	auto cos_theta_max = std::sqrt(1 - radius * radius / dist_squared);
	auto solid_angle = 2 * pi * (1 - cos_theta_max);

	return  1 / solid_angle;
}

vec3 sphere::random(const point3& origin) const {
	vec3 direction = center - origin;
	auto distance_squared = direction.length_squared();
	onb uvw(direction);
	return uvw.transform(random_to_sphere(radius, distance_squared));
}

vec3 sphere::random_to_sphere(double radius, double distance_squared) {
	auto r1 = random_double();
	auto r2 = random_double();
	auto z = 1 + r2 * (std::sqrt(1 - radius * radius / distance_squared) - 1);

	auto phi = 2 * pi * r1;
	auto x = std::cos(phi) * std::sqrt(1 - z * z);
	auto y = std::sin(phi) * std::sqrt(1 - z * z);

	return vec3(x, y, z);
}

point3 sphere::get_center() const {
	return center;
}