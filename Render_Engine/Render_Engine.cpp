#include "world.h"

#include <chrono>

static void show_time(double elapsed_ms) {
    auto total_ms = static_cast<long long>(elapsed_ms);

    long long hours = total_ms / 3'600'000;
    long long minutes = (total_ms / 60'000) % 60;
    long long seconds = (total_ms / 1'000) % 60;
    long long milliseconds = total_ms % 1'000;

    std::clog << "CPU time used: "
        << std::setfill('0')
        << std::setw(2) << hours << "h:"
        << std::setw(2) << minutes << "m:"
        << std::setw(2) << seconds << "s:"
        << std::setw(3) << milliseconds << "ms:"
        << '\n';
}

int main() {

    const std::clock_t c_start = std::clock();
    auto t_start = std::chrono::high_resolution_clock::now();

    world w;
    w.build();
    w.render();

    const std::clock_t c_end = std::clock();
    show_time(1000.0 * (c_end - c_start) / CLOCKS_PER_SEC);

    return 0;
}