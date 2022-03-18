from Adafruit_IO import MQTTClient
import sys
import time
import random

AIO_FEED_ID = "temp-device"
AIO_USERNAME =  "doantaydo"
AIO_KEY = "aio_XQVo47rUZ4j6QVoCMnB3wBAuBDtE"


def connected(client):
    print("Connected successfully ...")
    client.subscribe(AIO_FEED_ID)

def subscribe(client, userdata, mid, granted_qos):
    print("Subcribed successfully ...")

def disconnected(client):
    print("Disconnect ...") 
    sys.exit(1)

def message(client, feed_id, payload):
    print("Received data: " + payload)

client = MQTTClient(AIO_USERNAME, AIO_KEY)
client.on_connect = connected
client.on_disconnect = disconnected
client.on_message = message
client.on_subscribe = subscribe
client.connect ()
client.loop_background ()    

while True:
    value = random.randint(0, 100)
    #value = input("Input value: ")
    print("Update:", value)
    client.publish("temp-device", value)
    time.sleep(10)