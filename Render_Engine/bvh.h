#pragma once

#include "aabb.h"
#include "shape.h"
#include "shape_list.h"

#include <memory>
#include <vector>

class bvh_node : public shape {
public:
    bvh_node(shape_list list);
    bvh_node(std::vector<std::shared_ptr<shape>>& objects, size_t start, size_t end);

    bool hit(const ray& r, interval ray_t, hit_record& rec) const override;
    aabb bounding_box() const override;

private:
    std::shared_ptr<shape> left;
    std::shared_ptr<shape> right;
    aabb bbox;

    static bool box_compare(
        const std::shared_ptr<shape> a,
        const std::shared_ptr<shape> b,
        int axis_index
    );

    static bool box_x_compare(const std::shared_ptr<shape> a,
        const std::shared_ptr<shape> b);

    static bool box_y_compare(const std::shared_ptr<shape> a,
        const std::shared_ptr<shape> b);

    static bool box_z_compare(const std::shared_ptr<shape> a,
        const std::shared_ptr<shape> b);
};
