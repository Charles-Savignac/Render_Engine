#include "shape.h"

// ------------------------------------------------------------
// hit_record methods

void hit_record::set_face_normal(const ray& r, const vec3& outward_normal) {
	// outward_normal is assumed to be unit length
	front_face = dot(r.direction(), outward_normal) < 0;
	normal = front_face ? outward_normal : -outward_normal;
}

double shape::pdf_value(const point3& origin, const vec3& direction) const {
	return 0.0;
}

vec3 shape::random(const point3& origin) const {
	return vec3(1, 0, 0);
}