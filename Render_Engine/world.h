#pragma once

#include "bvh.h"
#include "camera.h"
#include "shape.h"
#include "shape_list.h"
#include "material.h"
#include "sphere.h"
#include "texture.h"
#include "quad.h"
#include "translate.h"
#include "rotate.h"

class world {
public:
	camera cam;
	shape_list scene;
	color background;

	void build() {
		background = BLACK;

		add_shapes();
		add_lights();
		add_camera();
	}

	void add_shapes() {
		auto red = make_shared<lambertian>(BLUE);
		auto white = make_shared<lambertian>(WHITE);
		auto green = make_shared<lambertian>(YELLOW);

		scene.add(make_shared<quad>(point3(555, 0, 0), vec3(0, 555, 0), vec3(0, 0, 555), green));
		scene.add(make_shared<quad>(point3(0, 0, 0), vec3(0, 555, 0), vec3(0, 0, 555), red));
		scene.add(make_shared<quad>(point3(0, 0, 0), vec3(555, 0, 0), vec3(0, 0, 555), white));
		scene.add(make_shared<quad>(point3(555, 555, 555), vec3(-555, 0, 0), vec3(0, 0, -555), white));
		scene.add(make_shared<quad>(point3(0, 0, 555), vec3(555, 0, 0), vec3(0, 555, 0), white));

		shared_ptr<shape> box1 = box(point3(0, 0, 0), point3(165, 330, 165), white);
		box1 = make_shared<rotate_y>(box1, 15);
		box1 = make_shared<translate>(box1, vec3(265, 0, 295));
		scene.add(box1);

		shared_ptr<shape> box2 = box(point3(0, 0, 0), point3(165, 165, 165), white);
		box2 = make_shared<rotate_y>(box2, -18);
		box2 = make_shared<translate>(box2, vec3(130, 0, 65));
		scene.add(box2);
	}

	void add_lights() {
		auto light = make_shared<diffuse_light>(color(15, 15, 15));
		scene.add(make_shared<quad>(point3(343, 554, 332), vec3(-130, 0, 0), vec3(0, 0, -105), light));
	}

	void add_camera() {
		cam.aspect_ratio = 1.0;
		cam.image_width = 600;
		cam.samples_per_pixel = 500;
		cam.max_depth = 10;

		cam.vfov = 40;
		cam.lookfrom = point3(278, 278, -800);
		cam.lookat = point3(278, 278, 0);
		cam.vup = vec3(0, 1, 0);

		cam.defocus_angle = 0;
	}

	void render() {
		cam.render(*this);
	}
};