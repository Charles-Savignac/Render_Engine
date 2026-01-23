#pragma once

#include "shape.h"
#include "shape_list.h"
#include "vec3.h"
#include "ray.h"
#include "aabb.h"
#include "interval.h"
#include <memory>

class quad : public shape {
public:
	quad(const point3& Q, const vec3& u, const vec3& v, std::shared_ptr<material> mat);

	aabb bounding_box() const override;
	bool hit(const ray& r, interval ray_t, hit_record& rec) const override;

	virtual void set_bounding_box();
	virtual bool is_interior(double a, double b, hit_record& rec) const;

private:
	point3 Q;
	vec3 u, v;
	vec3 w;
	vec3 normal;
	double D;
	std::shared_ptr<material> mat;
	aabb bbox;
};

// Helper function for constructing a 3D box (six quads)
std::shared_ptr<shape_list> box(const point3& a, const point3& b, std::shared_ptr<material> mat);
