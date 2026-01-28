#pragma once
#include "shape_list.h"
#include "onb.h"

class pdf {
public:
	virtual ~pdf() {}

	virtual double value(const vec3& direction) const = 0;
	virtual vec3 generate() const = 0;
};

class sphere_pdf : public pdf {
public:
	sphere_pdf();

	double value(const vec3& direction) const override;
	vec3 generate() const override;
};

class cosine_pdf : public pdf {

	onb uvw;

public:
	cosine_pdf(const vec3& w);

	double value(const vec3& direction) const override;
	vec3 generate() const override;

};

class shape_pdf : public pdf {

	const shape& objects;
	point3 origin;

public:
	shape_pdf(const shape& objects, const point3& origin);
	double value(const vec3& direction) const override;
	vec3 generate() const override;



};

class mixture_pdf : public pdf {
	shared_ptr<pdf> p[2];
public:
	mixture_pdf(shared_ptr<pdf> p0, shared_ptr<pdf> p1);
	double value(const vec3& direction) const override;

	vec3 generate() const override;
};