#include <iostream>
#include <string>

class Usuario {
private:
    string username;
    string email;
    string password;

public:
    // Construtor
    User(const string &user, const string &mail, const string &pass)
        : username(user), email(mail), password(pass) {}

    string getUsername() const {
        return username;
    }

    void setUsername(const string &user) {
        username = user;
    }

    string getEmail() const {
        return email;
    }

    void setEmail(const string &mail) {
        email = mail;
    }

    string getPassword() const {
        return password;
    }

    void setPassword(const string &pass) {
        password = pass;
    }

    void displayInfo() const {
        cout << "Username: " << username << ", Email: " << email << endl;
    }
};