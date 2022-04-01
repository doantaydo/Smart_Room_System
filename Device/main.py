from Adafruit_IO import MQTTClient, Client, Feed, Data
import sys
import time
import json
import random
import serial
import serial.tools.list_ports

AIO_FEED_DEVICE = ["microbit-led", "microbit-fan", "microbit-curtain"]
AIO_FEED_SENSOR = ["microbit-temp", "microbit-humid", "microbit-light", "microbit-gas"]

AIO_USERNAME = "HanhHuynh"
AIO_KEY = "aio_ZbJW476zKJY44M9M0EskA9x1fnh2"


def connected(client):
    print("Connected successfully ...")
    for feed in AIO_FEED_DEVICE:
        client.subscribe(feed)

def subscribe(client, userdata, mid, granted_qos):
    print("Subscribed successfully ...")


def disconnected(client):
    print("Disconnect ...")
    sys.exit(1)

def device(led, fan, curtain):
    data_x = {
        "LED": led,
        "FAN": fan,
        "CURTAIN": curtain
    }
    data_y = json.dumps(data_x)
    print(data_y)

aio = Client(AIO_USERNAME, AIO_KEY)

dataLed = aio.receive("microbit-led")
Led = int(dataLed.value)
dataFan = aio.receive("microbit-fan")
Fan = int(dataFan.value)
dataCurtain = aio.receive("microbit-curtain")
Curtain = int(dataCurtain.value)

def message(client, feed_id, payload):

    if feed_id == "microbit-led":
        device(payload, Fan, Curtain)
    if feed_id == "microbit-fan":
        device(Led, payload, Curtain)
    if feed_id == "microbit-curtain":
        device(Led, Fan, payload)        
    # if isMicrobitConnected:
    #     ser.write((str(payload) + "#").encode())

client = MQTTClient(AIO_USERNAME, AIO_KEY)
client.on_connect = connected
client.on_disconnect = disconnected
client.on_message = message
client.on_subscribe = subscribe
client.connect()
client.loop_background()

def handleFan(temp):
    if temp >= 40:
        client.publish("microbit_fan", 2)
    elif temp <= 10:
        client.publish("microbit_fan", 3)  

def getPort():
    # ports = serial.tools.list_ports.comports()
    # N = len(ports)
    # commPort = "None"
    # for i in range(0, N):
    #     port = ports[i]
    #     strPort = str(port)
    #     if "com0com" in strPort:
    #         splitPort = strPort.split(" ")
    #         commPort = (splitPort[0])
    # return commPort
    return "COM7"

isMicrobitConnected = False
if getPort() != "None":
    ser = serial.Serial(port=getPort(), baudrate=115200)
    isMicrobitConnected = True
 
mess = ""
def processData(data):
    data = data.replace("!", "")
    data = data.replace("#", "")
    splitData = data.split(":")
    print(splitData)
    try:
        if splitData[0] == "1":
            if splitData[1] == "TEMP":
                client.publish("microbit-temp", splitData[2])
                handleFan(int(splitData[2]))
            elif splitData[1] == "HUMI":
                client.publish("microbit-humid", splitData[2])   
        elif splitData[0] == "2":
            if splitData[1] == "LED":
                client.publish("microbit-led", splitData[2])        
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

def test():
    temp = random.randint(0, 50)
    humi = random.randint(0, 100)
    light = random.randint(0, 1023)
    gas = random.randint(0, 1023)
    client.publish("microbit-temp", temp)
    client.publish("microbit-humid", humi)
    client.publish("microbit-light", light)
    client.publish("microbit-gas", gas)

    handleFan(temp)

    data_x = {
        "Temp": temp,
        "Humid": humi,
        "Light": light,
        "Gas": gas  
    }

    data_y = json.dumps(data_x)
    print("Time:", time.ctime(time.time()))
    print(data_y)

    time.sleep(10)

while True:
    # test()

    if isMicrobitConnected:
        readSerial()

    time.sleep(1)
