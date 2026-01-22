#include "bvh.h"
#include "camera.h"
#include "shape.h"
#include "shape_list.h"
#include "material.h"
#include "sphere.h"
#include "texture.h"
#include "quad.h"

#include <chrono>

static void show_time(double elapsed_ms) {
    auto total_ms = static_cast<long long>(elapsed_ms);

    long long hours = total_ms / 3'600'000;
    long long minutes = (total_ms / 60'000) % 60;
    long long seconds = (total_ms / 1'000) % 60;
    long long milliseconds = total_ms % 1'000;

    std::clog << "CPU time used: "
        << std::setfill('0')
        << std::setw(2) << hours << "h:"
        << std::setw(2) << minutes << "m:"
        << std::setw(2) << seconds << "s:"
        << std::setw(3) << milliseconds << "ms:"
        << '\n';
}

int main() {

    const std::clock_t c_start = std::clock();
    auto t_start = std::chrono::high_resolution_clock::now();
    shape_list world;

    //auto ground_material = make_shared<lambertian>(color(0.5, 0.5, 0.5));
    //world.add(make_shared<sphere>(point3(0, -1000, 0), 1000, ground_material));

    //for (int a = -11; a < 11; a++) {
    //    for (int b = -11; b < 11; b++) {
    //        auto choose_mat = random_double();
    //        point3 center(a + 0.9 * random_double(), 0.2, b + 0.9 * random_double());

    //        if ((center - point3(4, 0.2, 0)).length() > 0.9) {
    //            shared_ptr<material> sphere_material;

    //            if (choose_mat < 0.8) {
    //                // diffuse
    //                auto albedo = color::random() * color::random();
    //                sphere_material = make_shared<lambertian>(albedo);
    //                world.add(make_shared<sphere>(center, 0.2, sphere_material));
    //            }
    //            else if (choose_mat < 0.95) {
    //                // metal
    //                auto albedo = color::random(0.5, 1);
    //                auto fuzz = random_double(0, 0.5);
    //                sphere_material = make_shared<metal>(albedo, fuzz);
    //                world.add(make_shared<sphere>(center, 0.2, sphere_material));
    //            }
    //            else {
    //                // glass
    //                sphere_material = make_shared<dielectric>(1.5);
    //                world.add(make_shared<sphere>(center, 0.2, sphere_material));
    //            }
    //        }
    //    }
    //}

    //auto material1 = make_shared<dielectric>(1.5);
    //world.add(make_shared<sphere>(point3(0, 1, 0), 1.0, material1));

    //auto material2 = make_shared<lambertian>(color(0.4, 0.2, 0.1));
    //world.add(make_shared<sphere>(point3(-4, 1, 0), 1.0, material2));

    //auto material3 = make_shared<metal>(color(0.7, 0.6, 0.5), 0.0);
    //world.add(make_shared<sphere>(point3(4, 1, 0), 1.0, material3));

    //world = shape_list(make_shared<bvh_node>(world));

    //camera cam;

    //cam.aspect_ratio = 16.0 / 9.0;
    //cam.image_width = 1200;
    //cam.samples_per_pixel = 1;
    //cam.max_depth = 50;

    //cam.vfov = 20;
    //cam.lookfrom = point3(13, 2, 3);
    //cam.lookat = point3(0, 0, 0);
    //cam.vup = vec3(0, 1, 0);

    //cam.defocus_angle = 0.6;
    //cam.focus_dist = 10.0;

    //========================================================================================================================================
    auto texture_checkered = make_shared<checker_texture>(0.5, color(.1, .1, .1), color(.9, .9, .9));

    auto material_ground = make_shared<lambertian>(color(0.8, 0.8, 0.0));
    auto material_center = make_shared<lambertian>(color(0.1, 0.2, 0.5));
    auto material_left = make_shared<dielectric>(1.50);
    auto material_bubble = make_shared<dielectric>(1.00 / 1.50);
    auto material_right = make_shared<metal>(color(0.8, 0.6, 0.2), 1.0);

    world.add(make_shared<sphere>(point3(0.0, -100.5, -1.0), 100.0, make_shared<lambertian>(texture_checkered)));
    world.add(make_shared<sphere>(point3(0.0, 0.0, -1.2), 0.5, material_center));
    world.add(make_shared<sphere>(point3(-1.0, 0.0, -1.0), 0.5, material_left));
    world.add(make_shared<sphere>(point3(-1.0, 0.0, -1.0), 0.4, material_bubble));
    world.add(make_shared<sphere>(point3(1.0, 0.0, -1.0), 0.5, material_right));

    world = shape_list(make_shared<bvh_node>(world));

    camera cam;

    cam.aspect_ratio = 16.0 / 9.0;
    cam.image_width = 1080;
    cam.samples_per_pixel = 50;
    cam.max_depth = 50;


    cam.vfov = 25;
    cam.lookfrom = point3(-2, 2, 1);
    cam.lookat = point3(0, 0, -1);
    cam.vup = vec3(0, 1, 0);


    cam.defocus_angle = 0;
    cam.focus_dist = 3.4;

    cam.render(world);

    const std::clock_t c_end = std::clock();
    show_time(1000.0 * (c_end - c_start) / CLOCKS_PER_SEC);

    return 0;
}