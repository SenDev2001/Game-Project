from flask import Flask, jsonify, request
from flask_cors import CORS

app = Flask(__name__)

CORS(app)


leaderboard = []

@app.route('/addscore', methods=['POST'])
def add_score():
    data = request.get_json()  # Get the JSON data from the request

    player_name = data.get('name')  # Get player name from the data
    player_score = data.get('score')  # Get player score from the data

    if not player_name or not player_score:
        return jsonify({"error": "Name and score are required"}), 400

    # Add the score to the leaderboard
    leaderboard.append({"name": player_name, "score": player_score})

    # Optionally return the updated leaderboard with a success message
    return jsonify({
        "message": "Score added successfully!",
        "player": player_name,
        "score": player_score,
        "leaderboard": leaderboard  # Return the current leaderboard
    })

@app.route('/leaderboard', methods=['GET'])
def get_leaderboard():
    # Sort the leaderboard by score in descending order
    sorted_leaderboard = sorted(leaderboard, key=lambda x: x['score'], reverse=True)

    # Return the sorted leaderboard as JSON
    return jsonify(sorted_leaderboard)

if __name__ == '__main__':
    # Run the Flask app
    app.run(debug=True, host='0.0.0.0', port=5003)
