#pragma once

#include "camera.h"
#include "tracer.h"
#include "shape_list.h"
#include "sampler.h"

class world {
public:
    camera cam;
    shape_list scene;
    shape_list lights;
    color background;
    std::unique_ptr<tracer> tracer_type;
    std::unique_ptr<sampler> sampler_type;

    ~world();

    void build();
    void add_shapes();
    void add_lights();
    void add_camera();
    void render();
};
