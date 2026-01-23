#pragma once

#include "shape.h"
#include "vec3.h"
#include "ray.h"
#include "aabb.h"
#include <memory>

class rotate_y : public shape {
public:
	// Constructors
	rotate_y(std::shared_ptr<shape> object, double angle);

	// Shape interface
	bool hit(const ray& r, interval ray_t, hit_record& rec) const override;
	aabb bounding_box() const override;

private:
	std::shared_ptr<shape> object;
	double sin_theta;
	double cos_theta;
	aabb bbox;
};
