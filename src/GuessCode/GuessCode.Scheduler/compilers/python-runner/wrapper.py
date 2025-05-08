# ---USER_CODE---
{{USER_CODE}}

import time, tracemalloc

def run_tests():
    with open("input.txt") as f:
        lines = [line.strip() for line in f.readlines()]
    test_count = int(lines[1])
    func_name = lines[2]
    inputs, outputs = [], []
    i = 3
    while i < len(lines):
        if lines[i] == "---INPUT---":
            inputs.append(eval(lines[i + 1]))
            i += 2
        elif lines[i] == "---OUTPUT---":
            outputs.append(eval(lines[i + 1]))
            i += 2
        else:
            i += 1

    with open("output.txt", "w") as out:
        for idx, (args, expected) in enumerate(zip(inputs, outputs), 1):
            start = time.time()
            tracemalloc.start()
            try:
                result = eval(f"{func_name}(*{args})")
                status = "PASSED" if result == expected else "FAILED"
            except Exception:
                status = "FAILED"
            current, peak = tracemalloc.get_traced_memory()
            tracemalloc.stop()
            elapsed = int((time.time() - start) * 1000)
            out.write(f"{idx} {status} TIME:{elapsed}ms MEMORY:{peak // 1024 // 1024}mb\n")

if __name__ == "__main__":
    run_tests()
