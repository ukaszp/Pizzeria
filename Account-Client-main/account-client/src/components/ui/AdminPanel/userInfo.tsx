import { useEffect, useState } from "react";
import useUserStore from "@/scripts/usersStore";
import { Link, useParams } from "react-router-dom";
import User from "../userInterface";
import { XSquare } from "lucide-react";
import { Avatar } from "@material-tailwind/react";
import MaleAvatar from "@/assets/male_avatar.svg";
import FemaleAvatar from "@/assets/female_avatar.svg";
import { date } from "yup";
import { APPLICATION_ROLES } from "@/config";

const UserInfo = () => {
  const { id } = useParams();
  const { getUserById } = useUserStore();
  const [currentUser, setCurrentUser] = useState<User | null>(null);

  useEffect(() => {
    const fetchUser = async () => {
      const maybeUser: User | null = await getUserById(Number(id));

      if (maybeUser !== null) {
        const user: User = maybeUser;
        setCurrentUser(user);
      } else {
        setCurrentUser(null);
      }
    };

    fetchUser();
  }, [id, getUserById]);

  const datestring = { time: currentUser?.whenJoined };
  let dateObject = null;

  if (datestring.time) {
    dateObject = new Date(datestring.time);
  } else {
    console.error("Invalid or undefined date string");
  }

  return (
    <div className="w-[70rem] bg-opacity-90 bg-primary rounded-xl p-7  mt-0 pt-0 pr-4 text-secondary justify-center max-h-[42rem] min-h-[40rem]">
      <div className="flex flex-col flex-1 text-secondary">
        <div className="self-end mt-3">
          <Link className="mt-0 pt-0" to="/admin/panel/users">
            <XSquare className="opacity-80 hover:opacity-100" />
          </Link>
        </div>
        <div className="m-12 border-[0.2rem] mx-[7rem] rounded-md">
          <div className="profileBackground h-[5rem]">
            <Avatar
              src={currentUser?.gender === true ? MaleAvatar : FemaleAvatar}
              alt="avatar"
              variant="rounded"
              size="lg"
              className="mt-10 bg-slate-600 border-[0.2rem] rounded-full w-[4.5rem] [4.5rem] mx-6"
            ></Avatar>
          </div>
          <div className="flex flex-row justify-center p-3 mt-2 text-xl font-bold">
            <div className="flex flex-row justify-center border-b-2">
              <p className="p-1 uppercase">{currentUser?.name}</p>
              <p className="p-1 uppercase ">{currentUser?.lastName}</p>
            </div>
          </div>
          <div>
            <div className="flex flex-row justify-center mt-[2rem] mb-[2rem]">
              <div className="flex-col ml-[12rem]">
                <div className="flex flex-col my-2">
                  <span className="font-semibold text-md">Email</span>
                  <p className="bg-white bg-opacity-10 p-2 my-1 text-gray-200 rounded-md">
                    {currentUser?.email}
                  </p>
                </div>
                <div className="flex flex-col my-2">
                  <span className="font-semibold text-md">Contact number</span>
                  <p className="bg-white bg-opacity-10 p-2 my-1 text-gray-200 rounded-md">
                    {currentUser?.contactNumber}
                  </p>
                </div>
                <div className="flex flex-col my-2">
                  <span className="font-semibold text-md">Gender</span>
                  {currentUser?.gender === true ? (
                    <p className="bg-white bg-opacity-10 p-2 my-1 text-gray-200 rounded-md">
                      Male
                    </p>
                  ) : (
                    <p className="bg-white bg-opacity-10 p-2 my-1 text-gray-200 rounded-md">
                      Female
                    </p>
                  )}
                </div>
                <Link
                  className="mt-10 transition ease-in-out duration-300 uppercase flex w-full lg:w-auto justify-center rounded-md bg-primary border-2 px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-secondary hover:text-primary focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
                  to={`/admin/panel/users/profile/edit/${currentUser?.id}`}
                >
                  Edit user
                </Link>
              </div>

              <div className="flex flex-col">
                <div className="flex flex-col mx-[10rem] my-2">
                  <span className="font-semibold text-md">Joined</span>
                  <p className="bg-white bg-opacity-10 p-2 my-1 text-gray-200 rounded-md">
                    {dateObject ? (
                      <div>{dateObject.toLocaleDateString()}</div>
                    ) : (
                      <div>invalid data</div>
                    )}
                  </p>
                  <div className="flex flex-col my-2">
                    <span className="font-semibold text-md">Role</span>
                    {currentUser?.roleId === APPLICATION_ROLES.ADMIN ? (
                      <p className="bg-white bg-opacity-10 p-2 my-1 text-gray-200 rounded-md">
                        Admin
                      </p>
                    ) : (
                      <p className="bg-white bg-opacity-10 p-2 my-1 text-gray-200 rounded-md">
                        User
                      </p>
                    )}
                  </div>
                  <div className="flex flex-col my-2">
                    <span className="font-semibold text-md">{"‎"}</span>
                    <p className="bg-white bg-opacity-10 p-2 my-1 text-gray-200 rounded-md">
                      {"‎"}
                    </p>
                  </div>
                  <Link
                  className="mt-6 transition ease-in-out duration-300 uppercase flex w-full lg:w-auto justify-center rounded-md bg-primary border-2 px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-secondary hover:text-primary focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
                  to={`admin/panel/profile/edit/${currentUser?.id}`}
                >
                  Assign role
                </Link>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default UserInfo;
