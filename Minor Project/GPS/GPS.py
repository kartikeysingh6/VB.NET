from bs4 import BeautifulSoup as BS     #For Website Grabbing
import csv                              #To Externally Store
import requests                         #For Sending Requests
import winsound                         #For Beeping Sounds (pip install )
import win10toast                       #For Desktop Notifications
import time                             #For loop or Sleep
import os.path                          #To Check if file exits or not
from twilio.rest import Client          #For Twilio API
from datetime import datetime           #For date and time

#Twilio API Information
#To send on whatsapp instead of sms(by default)
#use "whatsapp:+917985414735" and from should be a sandbox whatsapp
account_sid = "AC3d156f0265d90fd04876279b30fec24a"
auth_token  = "2dff90cb983d24cdefdfa0a2d72195cc"
frm="whatsapp:+14155238886"
to="whatsapp:+917985414735"

#www.twilio.com/referral/Sn601T

#API Function
def SENDAPI(text):
    client = Client(account_sid, auth_token)
    message = client.messages.create(to=to, from_=frm,body=text)
k=1
while(1):
    k-=1
    #City Name from city.txt if fails default is delhi
    if(os.path.exists('city.txt')):
        city_file=open("city.txt","r")
        city=city_file.readline().lower()
        city_file.close()
    else:
        city="delhi"

    print("Looking for gold price in "+city.capitalize()+"...")

    #Try and Except for Error Handling
    try:
        
        #Grabs data from the site and parses the HTML
        data = requests.get("https://www.fresherslive.com/gold-rate-today/"+city.lower()) 
        soup = BS(data.text, 'html.parser') 

        
        #To locate the data from site
        price = soup.findAll("td",class_="center-text")[1].text
        change = soup.find_all('b')[5].text

        #Statement to print
        final="1g 22k Price in " + city.upper().capitalize()+ ": "+price+" change: "+change
        print(final)

        
        #For external file
        dataExistance = os.path.exists('data.csv')
        now=datetime.now()
        datenow=now.strftime("%m-%d-%Y")
        timenow=now.strftime("%H:%M:%S") 
        header = ['City','Date', 'Time', 'Price', 'Change']
        data = [city.upper().capitalize(), datenow, timenow, "Rs."+price.split('₹')[1] ,change]


        with open('data.csv', 'a', encoding='UTF8', newline='') as f:
            writer = csv.writer(f)

            if(dataExistance==False):
                # write the header only if file doesn't exist
                writer.writerow(header)

            # write the data
            writer.writerow(data)

        
        #For sound effect, desktop notification and WhatsApp/SMS Alert
        if(change[0]=="-"):
            winsound.Beep(2000, 1000)
            if(os.path.exists('gold.ico')):
                toaster = win10toast.ToastNotifier().show_toast("Gold Price DROPPED!",final , duration=5, icon_path="gold.ico")
            else:
                toaster = win10toast.ToastNotifier().show_toast("Gold Price DROPPED!",final , duration=5)
            
            try:
                #API FUNCTION CALL DISABLE THIS TO DISABLE API
                #SENDAPI("Gold Price DROPPED!\n"+final)
                print()
            except:
                print("\nWhatsApp Alert Failed!")

        if(change[0]=="+"):
            winsound.Beep(1000, 350)
            winsound.Beep(1000, 350)
            if(os.path.exists('gold.ico')):
                toaster = win10toast.ToastNotifier().show_toast("Gold Price INCREASED!",final , duration=5, icon_path="gold.ico")
            else:
                toaster = win10toast.ToastNotifier().show_toast("Gold Price INCREASED!",final , duration=5)

            try:
                #API FUNCTION CALL DISABLE THIS TO DISABLE API
                #SENDAPI("Gold Price INCREASED!\n"+final)
                print()
            except:
                print("\nWhatsApp Alert Failed!")

    except Exception as e:
        print("City Not Found!")

    time.sleep(5)      #Loops over every given seconds