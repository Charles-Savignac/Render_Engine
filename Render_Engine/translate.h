#pragma once

#include "shape.h"
#include "vec3.h"
#include "ray.h"
#include "aabb.h"

class translate : public shape {
public:
	// Constructor
	translate(std::shared_ptr<shape> object, const vec3& offset);

	// Shape interface
	bool hit(const ray& r, interval ray_t, hit_record& rec) const override;
	aabb bounding_box() const override;

private:
	std::shared_ptr<shape> object;
	vec3 offset;
	aabb bbox;
};
