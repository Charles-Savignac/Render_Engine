#pragma once

#include "shape.h"
#include "color.h"
#include "texture.h"

// Base material interface
class material {
public:
	virtual ~material() = default;

	virtual bool scatter(const ray& r_in, const hit_record& rec, color& attenuation, ray& scattered) const;
};

/// Lambertian (diffuse) material
class lambertian : public material {
public:
	//lambertian(const color& albedo);

	lambertian(const color& albedo);
	lambertian(shared_ptr<texture> tex);

	bool scatter(const ray& r_in, const hit_record& rec, color& attenuation, ray& scattered) const override;

private:
	shared_ptr<texture> tex;
};

/// Metal (reflective) material
class metal : public material {
public:
	explicit metal(const color& albedo, double fuzz);

	bool scatter(const ray& r_in, const hit_record& rec, color& attenuation, ray& scattered) const override;

private:
	color albedo;
	double fuzz;
};

/// Dielectric (glass-like) material
class dielectric : public material {
public:
	explicit dielectric(double refraction_index);

	bool scatter(const ray& r_in, const hit_record& rec, color& attenuation, ray& scattered) const override;

private:
	double refraction_index;

	static double reflectance(double cosine, double refraction_index);
};