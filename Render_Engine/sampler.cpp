#include "sampler.h"


sampler::sampler(int s) : samples_per_pixel(s) {
	sqrt_spp = int(sqrt(samples_per_pixel));
	pixel_samples_scale = 1.0 / (sqrt_spp * sqrt_spp);
	recip_sqrt_spp = 1.0 / sqrt_spp;
}

stratified::stratified(int s) : sampler(s) { }
uniform::uniform(int s) : sampler(s) {}

vec3 stratified::sample_square(int s_i, int s_j) const {
	// Returns the vector to a random point in the square sub-pixel specified by grid
	// indices s_i and s_j, for an idealized unit square pixel [-.5,-.5] to [+.5,+.5].

	auto px = ((s_i + random_double()) * recip_sqrt_spp) - 0.5;
	auto py = ((s_j + random_double()) * recip_sqrt_spp) - 0.5;

	return vec3(px, py, 0);
}

vec3 uniform::sample_square(int s_i, int s_j) const {
	// Returns the vector to the center of the (s_i, s_j) stratum
	// in the unit square pixel [-0.5, -0.5] to [0.5, 0.5].

	auto px = ((s_i + 0.5) * recip_sqrt_spp) - 0.5;
	auto py = ((s_j + 0.5) * recip_sqrt_spp) - 0.5;

	return vec3(px, py, 0);
}