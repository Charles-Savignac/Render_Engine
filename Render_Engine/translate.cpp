#include "translate.h"

// Constructor
translate::translate(std::shared_ptr<shape> object, const vec3& offset) : object(object), offset(offset) {
	bbox = object->bounding_box() + offset;
}

// hit method
bool translate::hit(const ray& r, interval ray_t, hit_record& rec) const {
	// Move the ray backwards by the offset
	ray offset_r(r.origin() - offset, r.direction());

	// Determine whether an intersection exists along the offset ray
	if (!object->hit(offset_r, ray_t, rec))
		return false;

	// Move the intersection point forwards by the offset
	rec.p += offset;

	return true;
}

// bounding_box method
aabb translate::bounding_box() const {
	return bbox;
}
