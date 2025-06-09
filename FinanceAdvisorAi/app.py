from flask import Flask, request, jsonify
from flask_cors import CORS
from sklearn.linear_model import LinearRegression
import pandas as pd
import numpy as np

app = Flask(__name__)
CORS(app, resources={r"/predict": {"origins": "http://localhost:5173"}})  # Allow frontend origin

@app.route('/predict', methods=['POST'])
def predict():
    try:
        # Get transaction data from request
        data = request.get_json()
        transactions = data.get('Transactions', [])

        if not transactions:
            return jsonify({'error': 'No transactions provided'}), 400

        # Prepare data for model
        df = pd.DataFrame(transactions)
        df['Date'] = pd.to_datetime(df['Date'])
        df['Days'] = (df['Date'] - df['Date'].min()).dt.days  # Convert dates to days since earliest

        X = df[['Days']].values  # Feature: days
        y = df['Amount'].values  # Target: amount

        # Train linear regression model
        model = LinearRegression()
        model.fit(X, y)

        # Predict spending 30 days from the latest transaction
        latest_day = X.max()
        future_day = latest_day + 30
        prediction = model.predict([[future_day]])

        return jsonify({'Prediction': float(prediction[0])})
    except Exception as e:
        return jsonify({'error': str(e)}), 500

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5001, debug=True)