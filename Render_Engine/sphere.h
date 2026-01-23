#pragma once

#include "shape.h"
#include "vec3.h"

class sphere : public shape {
public:
	// Constructor
	sphere(const point3& center, double radius, shared_ptr<material> mat);

	// shape interface
	bool hit(const ray& r, interval ray_t, hit_record& rec) const override;
	aabb bounding_box() const override;

	static void get_sphere_uv(const point3& p, double& u, double& v) {
		// p: a given point on the sphere of radius one, centered at the origin.
		// u: returned value [0,1] of angle around the Y axis from X=-1.
		// v: returned value [0,1] of angle from Y=-1 to Y=+1.
		//     <1 0 0> yields <0.50 0.50>       <-1  0  0> yields <0.00 0.50>
		//     <0 1 0> yields <0.50 1.00>       < 0 -1  0> yields <0.50 0.00>
		//     <0 0 1> yields <0.25 0.50>       < 0  0 -1> yields <0.75 0.50>

		auto theta = std::acos(-p.y());
		auto phi = std::atan2(-p.z(), p.x()) + pi;

		u = phi / (2 * pi);
		v = theta / pi;
	}

private:
	point3 center;
	double radius;
	shared_ptr<material> mat;
	aabb bbox;
};
