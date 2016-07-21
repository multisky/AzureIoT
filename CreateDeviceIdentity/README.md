# Azure에서 IoT 활용시 참고할만한 팁들

- [C#] [Azure IoT Hub에 Device ID를 자동으로 생성하는 소스 예제](/CreateDeviceIdentity/)
  - 소스 Main에서 지정된 수 만큼 "Device-00001"의 형식으로 Device ID를 등록함
  - 소스 중에서 [connectionString] 부분은 여러분의 Azure IoT Hub에서 iothubowner의 연결문자열로 변경해줘야 함.
  - 참고 링크 : [https://azure.microsoft.com/en-us/documentation/articles/iot-hub-csharp-csharp-getstarted/](https://azure.microsoft.com/en-us/documentation/articles/iot-hub-csharp-csharp-getstarted/)