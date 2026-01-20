#pragma once

#include "vec3.h"
#include "interval.h"

// Color is just an alias for vec3
using color = vec3;

// ------------------------------------------------------------
// Color space conversion function declarations


// ------------------------------------------------------------
// Output
double linear_to_gamma(double linear_component);

void write_color(std::ostream& out, color pixel_color);
