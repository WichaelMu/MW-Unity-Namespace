
#if !_DEBUG
#include <iostream>
#include <string>
#include <chrono>

struct PerformanceTimer
{
	

public:
	void StartTime();

	void PrintTime(const std::string& message = "");

private:
	std::chrono::duration<double> time;
	std::chrono::steady_clock::time_point first, second;

	std::chrono::duration<double> Now();

};

inline void PerformanceTimer::StartTime()
{
	first = std::chrono::high_resolution_clock::now();
}

inline void PerformanceTimer::PrintTime(const std::string& message)
{
	if (message == "")
	{
		std::cout << std::chrono::duration_cast<std::chrono::milliseconds>(Now()).count() << "ms.\n";
	}
	else
	{
		std::cout << message << " " << std::chrono::duration_cast<std::chrono::milliseconds>(Now()).count() << "ms.\n";
	}
}

inline std::chrono::duration<double> PerformanceTimer::Now()
{
	second = std::chrono::high_resolution_clock::now();

	time = second - first;
	return time;
}

#endif
