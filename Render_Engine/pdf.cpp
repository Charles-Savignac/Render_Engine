#include "pdf.h"

sphere_pdf::sphere_pdf() {}

double sphere_pdf::value(const vec3& direction) const  {
    return 1 / (4 * pi);
}

vec3 sphere_pdf::generate() const  {
    return random_unit_vector();
}



cosine_pdf::cosine_pdf(const vec3& w) : uvw(w) {}

double cosine_pdf::value(const vec3& direction) const  {
    auto cosine_theta = dot(unit_vector(direction), uvw.w());
    return std::fmax(0, cosine_theta / pi);
}

vec3 cosine_pdf::generate() const  {
    return uvw.transform(random_cosine_direction());
}



shape_pdf::shape_pdf(const shape& objects, const point3& origin): objects(objects), origin(origin) {}

double shape_pdf::value(const vec3& direction) const {
    return objects.pdf_value(origin, direction);
}

vec3 shape_pdf::generate() const  {
    return objects.random(origin);
}

mixture_pdf::mixture_pdf(shared_ptr<pdf> p0, shared_ptr<pdf> p1) {
    p[0] = p0;
    p[1] = p1;
}

double mixture_pdf::value(const vec3& direction) const  {
    return 0.5 * p[0]->value(direction) + 0.5 * p[1]->value(direction);
}

vec3 mixture_pdf::generate() const {
    if (random_double() < 0.5)
        return p[0]->generate();
    else
        return p[1]->generate();
}