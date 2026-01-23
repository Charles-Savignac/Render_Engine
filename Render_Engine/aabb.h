#pragma once

#include "vec3.h"
#include "ray.h"
#include "interval.h"

class aabb {
public:
	interval x, y, z;

	aabb();  // default constructor

	aabb(const interval& x, const interval& y, const interval& z);
	aabb(const point3& a, const point3& b);
	aabb(const aabb& box0, const aabb& box1);

	const interval& axis_interval(int n) const;
	bool hit(const ray& r, interval ray_t) const;
	int longest_axis() const;

	static const aabb empty;
	static const aabb universe;

private:
	void pad_to_minimums();
};

aabb operator+(const aabb& bbox, const vec3& offset);
aabb operator+(const vec3& offset, const aabb& bbox);
