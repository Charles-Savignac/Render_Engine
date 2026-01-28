#pragma once

#include "shape.h"
#include "vec3.h"
#include "onb.h"

class sphere : public shape {
public:
	sphere(const point3& center, double radius, shared_ptr<material> mat);

	point3 get_center() const override;
	aabb bounding_box() const override;
	vec3 random(const point3& origin) const override;
	static void get_sphere_uv(const point3& p, double& u, double& v);
	bool hit(const ray& r, interval ray_t, hit_record& rec) const override;
	double pdf_value(const point3& origin, const vec3& direction) const override;

private:
	point3 center;
	double radius;
	shared_ptr<material> mat;
	aabb bbox;

	static vec3 random_to_sphere(double radius, double distance_squared);
};
