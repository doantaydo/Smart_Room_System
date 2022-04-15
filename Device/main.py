from Adafruit_IO import MQTTClient, Client
import sys
import time
import json
import random
import serial
import serial.tools.list_ports

AIO_FEED_DEVICE = ["microbit-led", "microbit-fan", "microbit-curtain"]

AIO_USERNAME = "HanhHuynh"
AIO_KEY = "aio_zdAG58j9hv9WNCPX9nEcIN7lplQH"


def connected(client):
    print("Connected successfully ...")
    for feed in AIO_FEED_DEVICE:
        client.subscribe(feed)

def subscribe(client, userdata, mid, granted_qos):
    print("Subscribed successfully ...")


def disconnected(client):
    print("Disconnect ...")
    sys.exit(1)


def message(client, feed_id, payload):  
    print (feed_id + ": " + payload)      
    if isMicrobitConnected:
        ser.write(("!" + str(payload) + "#").encode())

client = MQTTClient(AIO_USERNAME, AIO_KEY)
client.on_connect = connected
client.on_disconnect = disconnected
client.on_message = message
client.on_subscribe = subscribe
client.connect()
client.loop_background()

def getPort():
    # ports = serial.tools.list_ports.comports()
    # N = len(ports)
    # commPort = "None"
    # for i in range(0, N):
    #     port = ports[i]
    #     strPort = str(port)
    #     if "USB Serial Device" in strPort:
    #         splitPort = strPort.split(" ")
    #         commPort = (splitPort[0])
    # return commPort
    return "COM7"

isMicrobitConnected = False
if getPort() != "None":
    ser = serial.Serial(port=getPort(), baudrate=115200)
    isMicrobitConnected = True
 
def processData(data):
    data = data.replace("!", "")
    data = data.replace("#", "")
    splitData = data.split(":")
    print(splitData)
    try:
        if splitData[0] == "1":
            if splitData[1] == "TEMP":
                client.publish("microbit-temp", splitData[2])
            elif splitData[1] == "HUMI":
                client.publish("microbit-humid", splitData[2])   
        elif splitData[0] == "2":
            if splitData[1] == "LIGHT":
                client.publish("microbit-light", splitData[2])   
        elif splitData[0] == "3":
            if splitData[1] == "GAS":
                client.publish("microbit-gas", splitData[2])               
    except: 
        pass        

mess = ""
def readSerial():
    bytesToRead = ser.inWaiting()
    if (bytesToRead > 0):
        global mess
        mess = mess + ser.read(bytesToRead).decode("UTF-8")
        while ("#" in mess) and ("!" in mess):
            start = mess.find("!")
            end = mess.find("#")
            processData(mess[start:end + 1])
            if (end == len(mess)):
                mess = ""
            else:
                mess = mess[end+1:]           


while True:
    if isMicrobitConnected:
        readSerial()

    time.sleep(1)
