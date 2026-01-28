#pragma once

#include "ray.h"
#include "color.h"

class world;

class tracer {
public:
	virtual ~tracer() = default;

	virtual color cast_ray(const ray& r, int depth, const world& w) const = 0;
};

class path_tracing : public tracer {
public:
	color cast_ray(const ray & r, int depth, const world & w) const override;
};

class ray_tracing : public tracer {
public:
	color cast_ray(const ray& r, int depth, const world& w) const override;
};

