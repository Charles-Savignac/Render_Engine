#include "world.h"

#include "bvh.h"
#include "sphere.h"
#include "quad.h"
#include "translate.h"
#include "rotate.h"

world::~world() = default;

void world::build() {
    background = BLACK;
    tracer_type = std::make_unique<ray_tracing>();
	sampler_type = std::make_unique<uniform>(1);

    add_shapes();
    add_lights();
    add_camera();

    scene = shape_list(make_shared<bvh_node>(scene));
}

void world::add_shapes() {
	auto red = make_shared<lambertian>(RED);
	auto white = make_shared<lambertian>(WHITE);
	auto green = make_shared<lambertian>(GREEN);
	auto blue = make_shared<lambertian>(BLUE);
	auto test = make_shared<metal>(color(0.7, 0.6, 0.5), 0.0);

	scene.add(make_shared<quad>(point3(555, 0, 0), vec3(0, 555, 0), vec3(0, 0, 555), green));
	scene.add(make_shared<quad>(point3(0, 0, 0), vec3(0, 555, 0), vec3(0, 0, 555), red));
	scene.add(make_shared<quad>(point3(0, 0, 0), vec3(555, 0, 0), vec3(0, 0, 555), white));
	scene.add(make_shared<quad>(point3(555, 555, 555), vec3(-555, 0, 0), vec3(0, 0, -555), white));
	scene.add(make_shared<quad>(point3(0, 0, 555), vec3(555, 0, 0), vec3(0, 555, 0), white));

	// Box
	shared_ptr<shape> box1 = box(point3(0, 0, 0), point3(165, 330, 165), test);
	box1 = make_shared<rotate_y>(box1, 15);
	box1 = make_shared<translate>(box1, vec3(265, 0, 295));
	scene.add(box1);

	// Glass Sphere
	auto glass = make_shared<dielectric>(1.5);
	scene.add(make_shared<sphere>(point3(190, 90, 190), 90, glass));

}

void world::add_lights() {

	auto empty_material = shared_ptr<material>();
	auto light = make_shared<diffuse_light>(color(15, 15, 15));

	scene.add(make_shared<quad>(point3(343, 554, 332), vec3(-130, 0, 0), vec3(0, 0, -105), light));
	lights.add(make_shared<quad>(point3(343, 554, 332), vec3(-130, 0, 0), vec3(0, 0, -105), empty_material));
}

void world::add_camera() {

	cam.aspect_ratio = 1.0;
	cam.image_width = 800;
	cam.max_depth = 10;

	cam.lookfrom = point3(278, 278, -800);
	cam.lookat = point3(278, 278, 0);
	cam.vup = vec3(0, 1, 0);

	cam.vfov = 40;
	cam.defocus_angle = 0;
}

void world::render() {
	cam.render(*this);
}