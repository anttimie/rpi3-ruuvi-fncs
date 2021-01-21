#!/usr/bin/python3

import datetime
import json
import os

from ruuvitag_sensor.ruuvi import RuuviTagSensor
from azure.iot.device import IoTHubDeviceClient
import ruuvitag_sensor.log

CONNECTION_STRING = "ADD"
TIMEOUT = 5 # in seconds
MAC = ['ADD']

def create_device_client():
    device_client = IoTHubDeviceClient.create_from_connection_string(CONNECTION_STRING)
    return device_client

def send_sensor_data_iot_hub(payload):
    device_client = create_device_client()
    device_client.send_message(payload)

def main():
    ruuvitag_sensor.log.enable_console()

    sensor_data = RuuviTagSensor.get_data_for_sensors(MAC, TIMEOUT)

    json_sensor_data = json.dumps(sensor_data).encode()

    print(json_sensor_data)

    send_sensor_data_iot_hub(json_sensor_data)

if __name__ == '__main__':
    main()
