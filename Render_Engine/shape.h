#pragma once

#include "vec3.h"
#include "ray.h"
#include "interval.h"
#include "aabb.h"

class material;

class hit_record {
public:
	point3 p;
	vec3 normal;
	shared_ptr<material> mat;
	double t;
	double u;
	double v;
	bool front_face;

	void set_face_normal(const ray& r, const vec3& outward_normal);
};

class shape {
public:
	virtual ~shape() = default;
	virtual bool hit(const ray& r, interval ray_t, hit_record& rec) const = 0;
	virtual aabb bounding_box() const = 0;

	virtual point3 get_center() const {
		return point3(0, 0, 0);
	}

	virtual double pdf_value(const point3& origin, const vec3& direction) const;

	virtual vec3 random(const point3& origin) const;
};
