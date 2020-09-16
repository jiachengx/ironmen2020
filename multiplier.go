package main

import "C"
import "fmt"

//export helloworld
func helloworld() {
    fmt.Println("HelloWorld from DLL")
}

//export Sum
func Sum(a int, b int) int {
    return a + b;
}

//export Multiplier
func Multiplier(a int, b int) int {
    return a * b;
}

func main() {} 