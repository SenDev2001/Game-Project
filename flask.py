from flask import Flask, jsonify, request
from flask_cors import CORS

app = Flask(__name__)

# Enable CORS for all routes to allow cross-origin requests (e.g., from WebGL/Android)
CORS(app)

# Sample leaderboard (You can replace this with a database)
leaderboard = []

# Disable caching on the server side (add Cache-Control headers)
@app.after_request
def add_cache_control(response):
    response.cache_control.no_cache = True
    response.cache_control.no_store = True
    response.cache_control.must_revalidate = True
    response.headers['Pragma'] = 'no-cache'
    response.headers['Expires'] = '0'
    return response

# Route to add a score to the leaderboard
@app.route('/addscore', methods=['POST'])
def add_score():
    data = request.get_json()  # Get the JSON data from the request

    player_name = data.get('name')  # Get player name
    player_score = data.get('score')  # Get player score

    # Check if name and score are provided
    if not player_name or not player_score:
        return jsonify({"error": "Name and score are required"}), 400

    # Add the score to the leaderboard
    leaderboard.append({"name": player_name, "score": player_score})

    # Optionally, return the updated leaderboard along with a success message
    return jsonify({
        "message": "Score added successfully!",
        "player": player_name,
        "score": player_score,
        "leaderboard": leaderboard  # Return the current leaderboard
    })

# Route to get the leaderboard
@app.route('/leaderboard', methods=['GET'])
def get_leaderboard():
    # Sort the leaderboard by score in descending order
    sorted_leaderboard = sorted(leaderboard, key=lambda x: x['score'], reverse=True)

    # Return the sorted leaderboard as JSON
    return jsonify(sorted_leaderboard)

if __name__ == '__main__':
    # Run the Flask app
    app.run(debug=True, host='0.0.0.0', port=5003)
