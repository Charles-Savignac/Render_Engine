#include "camera.h"
#include "world.h"
#include "pdf.h"
#include "tracer.h"

#include <thread>
#include <vector>
#include <sstream>
#include <iostream>
#include <mutex>

void camera::render(const world& w) {
	initialize();

	const int num_threads = std::thread::hardware_concurrency() - 1;
	//const int num_threads = 1;

	std::vector<std::ostringstream> buffers(num_threads);
	std::clog << "task seperated on: " << num_threads << " threads\n\rScanlines remaining: ";

	std::atomic<int> rows_done{ 0 };
	// Worker function
	auto render_rows = [&](int thread_id, int start_row, int end_row) {
		for (int j = start_row; j < end_row; ++j) {
			for (int i = 0; i < image_width; ++i) {
				color pixel_color(0, 0, 0);

				for (int s_j = 0; s_j < w.sampler_type->sqrt_spp; s_j++) {
					for (int s_i = 0; s_i < w.sampler_type->sqrt_spp; s_i++) {
						ray r = get_ray(w, i, j, s_i, s_j);
						pixel_color += w.tracer_type->cast_ray(r, max_depth, w);
					}
				}

				write_color(buffers[thread_id], w.sampler_type->pixel_samples_scale * pixel_color);
			}
			++rows_done; // report progress
		}
		};

	// Progress reporter (single thread)
	std::thread progress_thread([&]() {
		while (rows_done < image_height) {
			std::clog << "\rScanlines remaining: "
				<< (image_height - rows_done.load())
				<< ' ' << std::flush;
			std::this_thread::sleep_for(std::chrono::milliseconds(1000));
		}
		});

	// Launch worker threads
	std::vector<std::thread> threads;
	threads.reserve(num_threads);

	int rows_per_thread = image_height / num_threads;
	int remaining_rows = image_height % num_threads;
	int current_row = 0;

	for (int t = 0; t < num_threads; ++t) {
		int start_row = current_row;
		int end_row = start_row + rows_per_thread + (t < remaining_rows ? 1 : 0);

		threads.emplace_back(render_rows, t, start_row, end_row);
		current_row = end_row;
	}

	// Wait for workers
	for (auto& th : threads)
		th.join();

	// Wait for progress reporter
	progress_thread.join();

	// Output image
	std::cout << "P3\n" << image_width << ' ' << image_height << "\n255\n";

	for (int t = 0; t < num_threads; ++t)
		std::cout << buffers[t].str();

	std::clog << "\rDone.                     \n";
}

void camera::initialize() {
	image_height = int(image_width / aspect_ratio);
	image_height = (image_height < 1) ? 1 : image_height;

	center = lookfrom;

	// Determine viewport dimensions
	auto theta = degrees_to_radians(vfov);
	auto h = std::tan(theta / 2);
	auto viewport_height = 2 * h * focus_dist;
	auto viewport_width = viewport_height * (double(image_width) / image_height);

	// Calculate the u,v,w unit basis vectors for the camera coordinate frame.
	w = unit_vector(lookfrom - lookat);
	u = unit_vector(cross(vup, w));
	v = cross(w, u);

	// Calculate the vectors across the horizontal and down the vertical viewport edges.
	vec3 viewport_u = viewport_width * u;    // Vector across viewport horizontal edge
	vec3 viewport_v = viewport_height * -v;  // Vector down viewport vertical edge

	// Calculate the horizontal and vertical delta vectors from pixel to pixel.
	pixel_delta_u = viewport_u / image_width;
	pixel_delta_v = viewport_v / image_height;

	// Calculate the location of the upper left pixel.
	auto viewport_upper_left = center - (focus_dist * w) - viewport_u / 2 - viewport_v / 2;
	pixel00_loc = viewport_upper_left + 0.5 * (pixel_delta_u + pixel_delta_v);

	// Calculate the camera defocus disk basis vectors.
	auto defocus_radius = focus_dist * std::tan(degrees_to_radians(defocus_angle / 2));
	defocus_disk_u = u * defocus_radius;
	defocus_disk_v = v * defocus_radius;
}

point3 camera::defocus_disk_sample() const {
	// Returns a random point in the camera defocus disk.
	auto p = random_in_unit_disk();
	return center + (p[0] * defocus_disk_u) + (p[1] * defocus_disk_v);
}

ray camera::get_ray(const world& w, int i, int j, int s_i, int s_j) const {
	// Construct a camera ray originating from the defocus disk and directed at a randomly
	// sampled point around the pixel location i, j.

	auto offset = w.sampler_type->sample_square(s_i, s_j);
	auto pixel_sample = pixel00_loc
		+ ((i + offset.x()) * pixel_delta_u)
		+ ((j + offset.y()) * pixel_delta_v);

	auto ray_origin = (defocus_angle <= 0) ? center : defocus_disk_sample();
	auto ray_direction = pixel_sample - ray_origin;

	return ray(ray_origin, ray_direction);
}