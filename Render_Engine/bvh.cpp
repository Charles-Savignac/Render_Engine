#include "bvh.h"

#include <algorithm>

bvh_node::bvh_node(shape_list list) : bvh_node(list.objects, 0, list.objects.size()) {

	// This constructor makes an implicit copy of the shape list.
	// The copy is modified to build the BVH, but only the resulting
	// hierarchy needs to persist.
}

bvh_node::bvh_node(std::vector<std::shared_ptr<shape>>& objects, size_t start, size_t end) {
	// Build bounding box for this node
	bbox = aabb::empty;
	for (size_t i = start; i < end; ++i)
		bbox = aabb(bbox, objects[i]->bounding_box());

	int axis = bbox.longest_axis();

	auto comparator =
		(axis == 0) ? box_x_compare :
		(axis == 1) ? box_y_compare :
		box_z_compare;

	size_t object_span = end - start;

	if (object_span == 1) {
		left = right = objects[start];
	}
	else if (object_span == 2) {
		left = objects[start];
		right = objects[start + 1];
	}
	else {
		std::sort(objects.begin() + start,
			objects.begin() + end,
			comparator);

		size_t mid = start + object_span / 2;
		left = std::make_shared<bvh_node>(objects, start, mid);
		right = std::make_shared<bvh_node>(objects, mid, end);
	}
}

bool bvh_node::hit(const ray& r, interval ray_t, hit_record& rec) const {
	if (!bbox.hit(r, ray_t))
		return false;

	bool hit_left = left->hit(r, ray_t, rec);
	bool hit_right = right->hit(
		r,
		interval(ray_t.min, hit_left ? rec.t : ray_t.max),
		rec
	);

	return hit_left || hit_right;
}

aabb bvh_node::bounding_box() const {
	return bbox;
}

bool bvh_node::box_compare(const std::shared_ptr<shape> a, const std::shared_ptr<shape> b, int axis_index) {
	auto a_interval = a->bounding_box().axis_interval(axis_index);
	auto b_interval = b->bounding_box().axis_interval(axis_index);
	return a_interval.min < b_interval.min;
}

bool bvh_node::box_x_compare(const std::shared_ptr<shape> a,
	const std::shared_ptr<shape> b) {
	return box_compare(a, b, 0);
}

bool bvh_node::box_y_compare(const std::shared_ptr<shape> a,
	const std::shared_ptr<shape> b) {
	return box_compare(a, b, 1);
}

bool bvh_node::box_z_compare(const std::shared_ptr<shape> a,
	const std::shared_ptr<shape> b) {
	return box_compare(a, b, 2);
}