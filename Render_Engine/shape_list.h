#pragma once

#include "shape.h"

#include <memory>
#include <vector>

class shape_list : public shape {
public:
    // Public members
    std::vector<std::shared_ptr<shape>> objects;

    // Constructors
    shape_list();
    explicit shape_list(std::shared_ptr<shape> object);

    // Modifiers
    void clear();
    void add(std::shared_ptr<shape> object);

    // shape interface
    bool hit(
        const ray& r,
        interval ray_t,
        hit_record& rec
    ) const override;
};
