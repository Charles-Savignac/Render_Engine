#include "color.h"

#include <cmath>
#include <string>

double linear_to_gamma(double linear_component) {
	if (linear_component > 0)
		return std::sqrt(linear_component);

	return 0;
}

color hex_to_color(const std::string& hex) {

	std::string hexcode = hex;
	// Remove leading '#' if present
	if (!hexcode.empty() && hexcode[0] == '#')
		hexcode.erase(0, 1);

	// Return black if hexcode is not 6 char long
	if (hexcode.length() != 6)
		return BLACK;

	int r = std::stoi(hexcode.substr(0, 2), nullptr, 16);
	int g = std::stoi(hexcode.substr(2, 2), nullptr, 16);
	int b = std::stoi(hexcode.substr(4, 2), nullptr, 16);

	return vec3(
		r / 255.0f,
		g / 255.0f,
		b / 255.0f
	);
}

void write_color(std::ostream& out, color pixel_color) {
	auto r = pixel_color.x();
	auto g = pixel_color.y();
	auto b = pixel_color.z();

	// Replace NaN components with zero.
	if (r != r) r = 0.0;
	if (g != g) g = 0.0;
	if (b != b) b = 0.0;

	// Apply a linear to gamma transform for gamma 2
	r = linear_to_gamma(r);
	g = linear_to_gamma(g);
	b = linear_to_gamma(b);

	// Translate the [0,1] component values to the byte range [0,255].
	static const interval intensity(0.000, 0.999);
	int rbyte = int(256 * intensity.clamp(r));
	int gbyte = int(256 * intensity.clamp(g));
	int bbyte = int(256 * intensity.clamp(b));

	// Write out the pixel color components.
	out << rbyte << ' ' << gbyte << ' ' << bbyte << '\n';
}
