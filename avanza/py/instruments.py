import json

class Instruments:
    _instance = None

    def __new__(cls):
        if cls._instance is None:
            cls._instance = super(Instruments, cls).__new__(cls)
            with open('instruments.json', 'r') as file:
                print("Fetching instruments from file...")
                cls._instance.data = json.load(file)
        return cls._instance