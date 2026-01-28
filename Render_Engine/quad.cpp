#include "quad.h"
#include <cmath>
#include "interval.h"

// Constructor
quad::quad(const point3& Q, const vec3& u, const vec3& v, std::shared_ptr<material> mat) : Q(Q), u(u), v(v), mat(mat) {
	auto n = cross(u, v);
	normal = unit_vector(n);
	D = dot(normal, Q);
	w = n / dot(n, n);
	area = n.length();
	set_bounding_box();
}

// Set the bounding box of the quad
void quad::set_bounding_box() {
	auto bbox_diagonal1 = aabb(Q, Q + u + v);
	auto bbox_diagonal2 = aabb(Q + u, Q + v);
	bbox = aabb(bbox_diagonal1, bbox_diagonal2);
}

// Bounding box
aabb quad::bounding_box() const {
	return bbox;
}

// Check for ray-quad intersection
bool quad::hit(const ray& r, interval ray_t, hit_record& rec) const {
	auto denom = dot(normal, r.direction());

	// No hit if ray is parallel
	if (std::fabs(denom) < 1e-8)
		return false;

	auto t = (D - dot(normal, r.origin())) / denom;
	if (!ray_t.contains(t))
		return false;

	auto intersection = r.at(t);
	vec3 planar_hitpt_vector = intersection - Q;
	auto alpha = dot(w, cross(planar_hitpt_vector, v));
	auto beta = dot(w, cross(u, planar_hitpt_vector));

	if (!is_interior(alpha, beta, rec))
		return false;

	rec.t = t;
	rec.p = intersection;
	rec.mat = mat;
	rec.set_face_normal(r, normal);

	return true;
}

// Check if intersection is inside quad
bool quad::is_interior(double a, double b, hit_record& rec) const {
	interval unit_interval(0, 1);
	if (!unit_interval.contains(a) || !unit_interval.contains(b))
		return false;

	rec.u = a;
	rec.v = b;
	return true;
}

// Construct a 3D box from two points
std::shared_ptr<shape_list> box(const point3& a, const point3& b, std::shared_ptr<material> mat) {
	auto sides = std::make_shared<shape_list>();

	auto min = point3(std::fmin(a.x(), b.x()), std::fmin(a.y(), b.y()), std::fmin(a.z(), b.z()));
	auto max = point3(std::fmax(a.x(), b.x()), std::fmax(a.y(), b.y()), std::fmax(a.z(), b.z()));

	auto dx = vec3(max.x() - min.x(), 0, 0);
	auto dy = vec3(0, max.y() - min.y(), 0);
	auto dz = vec3(0, 0, max.z() - min.z());

	sides->add(std::make_shared<quad>(point3(min.x(), min.y(), max.z()), dx, dy, mat)); // front
	sides->add(std::make_shared<quad>(point3(max.x(), min.y(), max.z()), -dz, dy, mat)); // right
	sides->add(std::make_shared<quad>(point3(max.x(), min.y(), min.z()), -dx, dy, mat)); // back
	sides->add(std::make_shared<quad>(point3(min.x(), min.y(), min.z()), dz, dy, mat)); // left
	sides->add(std::make_shared<quad>(point3(min.x(), max.y(), max.z()), dx, -dz, mat)); //_

	return sides;
}

double quad::pdf_value(const point3& origin, const vec3& direction) const {
	hit_record rec;
	if (!this->hit(ray(origin, direction), interval(0.001, infinity), rec))
		return 0;

	auto distance_squared = rec.t * rec.t * direction.length_squared();
	auto cosine = std::fabs(dot(direction, rec.normal) / direction.length());

	return distance_squared / (cosine * area);
}

vec3 quad::random(const point3& origin) const {
	return Q + (random_double() * u) + (random_double() * v) - origin;
}

point3 quad::get_center() const {
	return Q + 0.5 * u + 0.5 * v;
}