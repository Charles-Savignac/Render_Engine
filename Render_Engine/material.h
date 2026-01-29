#pragma once

#include "shape.h"
#include "color.h"
#include "texture.h"
#include "pdf.h"

class scatter_record {
public:
	color attenuation;
	shared_ptr<pdf> pdf_ptr;
	bool skip_pdf;
	ray skip_pdf_ray;
};

// Base material interface
class material {
public:
	virtual ~material() = default;

	virtual color emitted(const ray& r_in, const hit_record& rec, double u, double v, const point3& p) const {
		return color(0, 0, 0);
	}

	virtual bool scatter(const ray& r_in, const hit_record& rec, scatter_record& srec) const;
	virtual double scattering_pdf(const ray& r_in, const hit_record& rec, const ray& scattered) const;
};

/// Lambertian (diffuse) material
class lambertian : public material {
public:
	//lambertian(const color& albedo);
	lambertian(const color& albedo);
	lambertian(const std::string& hex);

	lambertian(shared_ptr<texture> tex);

	bool scatter(const ray& r_in, const hit_record& rec, scatter_record& srec) const override;
	double scattering_pdf(const ray& r_in, const hit_record& rec, const ray& scattered) const override;

private:
	shared_ptr<texture> tex;
};

/// Metal (reflective) material
class metal : public material {
public:
	explicit metal(const color& albedo, double fuzz);
	bool scatter(const ray& r_in, const hit_record& rec, scatter_record& srec) const override;

private:
	color albedo;
	double fuzz;
};

/// Dielectric (glass-like) material
class dielectric : public material {
public:
	explicit dielectric(double refraction_index);
	bool scatter(const ray& r_in, const hit_record& rec, scatter_record& srec) const override;

private:
	double refraction_index;

	static double reflectance(double cosine, double refraction_index);
};

class diffuse_light : public material {
public:
	diffuse_light(std::shared_ptr<texture> tex);
	diffuse_light(const color& emit);

	color emitted(const ray& r_in, const hit_record& rec, double u, double v, const point3& p) const override;

private:
	std::shared_ptr<texture> tex;
};