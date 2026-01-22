#include "texture.h"

solid_color::solid_color(const color& albedo) : albedo(albedo) {}

solid_color::solid_color(double red, double green, double blue) : solid_color(color(red, green, blue)) {}

color solid_color::value(double u, double v, const point3& p) const {
	return albedo;
}