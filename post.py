import requests as r

cmd = input()
url = 'http://127.0.0.1:5000/cmd'
data = {'command': f'{cmd}'}

r.post(url, data=data)
