#pragma once

#include "vec3.h"
#include <iostream>
#include <cmath>

using color = vec3;

static float srgb_to_linear(float c)
{
    if (c <= 0.04045f)
        return c / 12.92f;
    else
        return std::pow((c + 0.055f) / 1.055f, 2.4f);
}

static color sRGB_to_linear(const color& c)
{
    return { srgb_to_linear(c.x()), srgb_to_linear(c.y()), srgb_to_linear(c.z()) };
}

static float linear_to_srgb(float c)
{
    if (c <= 0.0031308f)
        return 12.92f * c;
    else
        return 1.055f * std::pow(c, 1.0f / 2.4f) - 0.055f;
}

static color linear_to_sRGB(const color& c)
{
    return { linear_to_srgb(c.x()), linear_to_srgb(c.y()), linear_to_srgb(c.z()) };
}


inline void write_color(std::ostream& out, color pixel_color, int samples_per_pixel = 1, float gamma = 1.0f)
{
    // Divide the color by the number of samples to get the average
    float scale = 1.0f / samples_per_pixel;
    pixel_color *= scale;

    // Apply gamma correction (linear → gamma space)
    pixel_color = {
        std::pow(pixel_color.x(), 1.0f / gamma),
        std::pow(pixel_color.y(), 1.0f / gamma),
        std::pow(pixel_color.z(), 1.0f / gamma)
    };

    // Convert to sRGB for output (optional, depending on your pipeline)
    pixel_color = linear_to_sRGB(pixel_color);

    // Convert to 0-255 bytes
    int rbyte = static_cast<int>(255.999 * pixel_color.x());
    int gbyte = static_cast<int>(255.999 * pixel_color.y());
    int bbyte = static_cast<int>(255.999 * pixel_color.z());

    out << rbyte << ' ' << gbyte << ' ' << bbyte << '\n';
}
