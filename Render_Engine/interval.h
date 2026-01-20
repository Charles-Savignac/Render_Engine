#pragma once

class interval {
public:
    double min, max;

    // Constructors
    interval();
    interval(double min, double max);

    // Methods
    double size() const;
    double clamp(double x) const;
    bool contains(double x) const;
    bool surrounds(double x) const;

    // Predefined intervals
    static const interval empty;
    static const interval universe;
};
