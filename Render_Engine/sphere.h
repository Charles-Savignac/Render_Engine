#pragma once

#include "shape.h"
#include "vec3.h"

class sphere : public shape {
public:
    // Constructor
    sphere(const point3& center, double radius, shared_ptr<material> mat);

    // shape interface
    bool hit(const ray& r,interval ray_t,hit_record& rec) const override;
    aabb bounding_box() const override;

private:
    point3 center;
    double radius;
    shared_ptr<material> mat;
    aabb bbox;
};
