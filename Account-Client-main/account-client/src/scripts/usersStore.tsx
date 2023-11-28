import { create } from "zustand";
import api from "./api";
import User from "@/components/ui/userInterface";
import { FormikHelpers } from "formik";
import { EditUserInterface } from "@/components/ui/AdminPanel/editUserinterface";

interface UserStore {
    users: User[];
    setUsers: (users: User[]) => void;
    getAllUsers: () => void;
    setSelectedUser: (user: User | null) => void;
    getUserById: (id: number) => Promise<User | null>;
    selectedUser: User | null;
    deleteUser: (id: number) => Promise<void>;
    editUser: (id: number, userData: Partial<User>, formikHelpers: FormikHelpers<any>) => Promise<void>;
  }

const useUserStore = create<UserStore>((set) => ({
    isAuthenticated: false,
    editError: null,

    users: [],
    setUsers: (users) => set({ users }),
    getAllUsers: async () => {
      try {
        const response = await api.get('/account/all');
        const users = response.data;
        set({ users });
      } catch (error) {
        console.error('no data', error);
      }
    },
    setSelectedUser: (user) => set({ selectedUser: user }),
    selectedUser: null,
    getUserById: async (id: number) => {
        try {
            const response = await api.get(`/account/${id}`);
            const user = response.data;
            return user;
        } catch (error) {
            console.error('no data', error);
        }
    }
    ,
    deleteUser: async (id: number) => {  return api.delete(`/account/${id}`) },
    editUser: async (id: number, userData: Partial<User>, formikHelpers: FormikHelpers<any>) => {
      try {
        const response = await api.put(`/account/${id}`, userData);
        const updatedUser = response.data;
  
        set((state) => {
          const updatedUsers = state.users.map((user) =>
            user.id === id ? { ...user, ...updatedUser } : user
          );
          return { users: updatedUsers };
        });
        formikHelpers.resetForm();
        formikHelpers.setStatus('User updated successfully');
      } catch (error) {
        console.error('Error updating user', error);
        formikHelpers.setStatus('Error updating user');
      }
    },
  
  }));

  export default useUserStore;
