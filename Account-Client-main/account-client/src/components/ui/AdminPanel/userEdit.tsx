import { useEffect, useState } from "react";
import useUserStore from "@/scripts/usersStore";
import { Link, useParams } from "react-router-dom";
import User from "../userInterface";
import { XSquare } from "lucide-react";
import { Avatar } from "@material-tailwind/react";
import MaleAvatar from "@/assets/male_avatar.svg";
import FemaleAvatar from "@/assets/female_avatar.svg";
import {useFormik} from "formik";
import * as Yup from "yup";
import { APPLICATION_ROLES } from "@/config";


const UserEdit = () => {
  const { id } = useParams();
  const { getUserById, editUser } = useUserStore();
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

  const formik = useFormik({
    initialValues: {
      name: currentUser?.name || "",
      lastname: currentUser?.lastName || "",
      email: currentUser?.email || "",
      contactnumber: currentUser?.contactNumber || "",
      gender: currentUser?.gender || false,
    },
    validationSchema: Yup.object({
      name: Yup.string()
      .required('*Name is required')
      .matches(/^[A-Za-z]+$/, "*Only letters are allowed"),
      lastname: Yup
      .string().required('*Lastname is required')
      .matches(/^[A-Za-z]+$/, "*Only letters are allowed"),
      email: Yup.string()
      .required('*Email is required')
      .email('*invalid email address'),
      contactnumber: Yup.string()
      .required('*phone number is required')
      .matches(/^[0-9]{9}( |)$/, "*Invalid phone number format")
      .max(9, '*Your number is too long')
      .min(9, '*You missed some digits'),
      password: Yup.string()
      .required('*enter your password')
      .min(5, '*Must be at least 5 characters'),
      confirmpassword: Yup.string()
        .oneOf([Yup.ref('password')], '*passwords are not the same')
        .required('*confirm your password'),
    }),
    onSubmit: async (values, formikHelpers) => {
      try {
        if (currentUser && currentUser.id) {
          const userId = currentUser.id;
          await useUserStore.getState().editUser(userId, { 
            name: values.name,
            lastName: values.lastname,
            email: values.email,
            contactNumber: values.contactnumber,
            // Add other fields as needed
          }, formikHelpers);
          console.log("values:"+values);
          console.log("formikHelpers:"+formikHelpers);
        } else {
          console.error('User ID not found');
        }         
      } catch (error) {
        console.error('Error updating user', error);
        formikHelpers.setStatus('Error updating user');
      }
    },
  });

  

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
            <form 
            className="flex flex-row justify-center mt-[2rem] mb-[2rem]"
            onSubmit={formik.handleSubmit}>
              <div className="flex-col ml-[12rem]">
                <div className="flex flex-col my-2">
                  <span className="font-semibold text-md">Email</span>
                  <input 
                  className="bg-white bg-opacity-10 p-2 my-1 text-gray-200 rounded-md"
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  />
                </div>
                <div className="flex flex-col my-2">
                  <span className="font-semibold text-md">Contact number</span>
                  <input 
                  className="bg-white bg-opacity-10 p-2 my-1 text-gray-200 rounded-md"
                  value={currentUser?.contactNumber}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  />
                </div>
                <div>
                  <label
                    htmlFor="gender"
                    className="block text-xs font-small leading-6 text-gray-100 uppercase"
                  >
                    Gender
                  </label>
                  <div className="mb-[0.125rem] block min-h-[1.5rem] pl-[1.5rem]">
                    <input
                      className="relative float-left -ml-[1.5rem] mr-1 mt-0.5 h-5 w-5 appearance-none rounded-full border-2 border-solid 
                    border-gray-500 before:pointer-events-none before:absolute before:h-4 before:w-4 before:scale-0 before:rounded-full before:bg-transparent 
                    before:opacity-0 before:shadow-[0px_0px_0px_13px_transparent] before:content-[''] after:absolute after:z-[1] after:block after:h-4 after:w-4 after:rounded-full after:content-['']
                     checked:border-white checked:before:opacity-[0.16] checked:after:absolute checked:after:left-1/2 checked:after:top-1/2 checked:after:h-[0.625rem] checked:after:w-[0.625rem] 
                     checked:after:rounded-full checked:after:border-primary checked:after:bg-secondary checked:after:content-[''] checked:after:[transform:translate(-50%,-50%)] 
                     hover:cursor-pointer hover:before:opacity-[0.04] hover:before:shadow-[0px_0px_0px_13px_rgba(0,0,0,0.6)] focus:shadow-none focus:outline-none focus:ring-0 focus:before:scale-100 
                     focus:before:opacity-[0.12] focus:before:shadow-[0px_0px_0px_13px_rgba(0,0,0,0.6)] focus:before:transition-[box-shadow_0.2s,transform_0.2s] 
                     checked:focus:border-blue-80 checked:focus:before:scale-100 checked:focus:before:shadow-[0px_0px_0px_13px_#789ad0] checked:focus:before:transition-[box-shadow_0.2s,transform_0.2s]
                      dark:border-neutral-600 dark:checked:border-primary dark:checked:after:border-primary dark:checked:after:bg-primary dark:focus:before:shadow-[0px_0px_0px_13px_
                        rgba(255,255,255,0.4)] dark:checked:focus:border-primary dark:checked:focus:before:shadow-[0px_0px_0px_13px_#3b71ca]"
                        type="radio"
                        name="flexRadioDefault"
                        id="radioDefault01" 
                        checked={formik.values.gender===true}
                        onChange={() => formik.setFieldValue('gender', true)}
                    />
                    <label
                      className="mt-px inline-block pl-[0.15rem] hover:cursor-pointer text-xs font-small leading-6 text-gray-100 uppercase"
                      htmlFor="radioDefault01"
                    >
                      Male
                    </label>
                  </div>
                  <div className="mb-[0.125rem] block min-h-[1.5rem] pl-[1.5rem]">
                    <input
                      className="relative float-left -ml-[1.5rem] mr-1 mt-0.5 h-5 w-5 appearance-none rounded-full border-2 border-solid 
                    border-gray-500 before:pointer-events-none before:absolute before:h-4 before:w-4 before:scale-0 before:rounded-full before:bg-transparent 
                    before:opacity-0 before:shadow-[0px_0px_0px_13px_transparent] before:content-[''] after:absolute after:z-[1] after:block after:h-4 after:w-4 after:rounded-full after:content-['']
                     checked:border-white checked:before:opacity-[0.16] checked:after:absolute checked:after:left-1/2 checked:after:top-1/2 checked:after:h-[0.625rem] checked:after:w-[0.625rem] 
                     checked:after:rounded-full checked:after:border-primary checked:after:bg-secondary checked:after:content-[''] checked:after:[transform:translate(-50%,-50%)] 
                     hover:cursor-pointer hover:before:opacity-[0.04] hover:before:shadow-[0px_0px_0px_13px_rgba(0,0,0,0.6)] focus:shadow-none focus:outline-none focus:ring-0 focus:before:scale-100 
                     focus:before:opacity-[0.12] focus:before:shadow-[0px_0px_0px_13px_rgba(0,0,0,0.6)] focus:before:transition-[box-shadow_0.2s,transform_0.2s] 
                     checked:focus:border-blue-80 checked:focus:before:scale-100 checked:focus:before:shadow-[0px_0px_0px_13px_#789ad0] checked:focus:before:transition-[box-shadow_0.2s,transform_0.2s]
                      dark:border-neutral-600 dark:checked:border-primary dark:checked:after:border-primary dark:checked:after:bg-primary dark:focus:before:shadow-[0px_0px_0px_13px_
                        rgba(255,255,255,0.4)] dark:checked:focus:border-primary dark:checked:focus:before:shadow-[0px_0px_0px_13px_#3b71ca]"

                        type="radio"
                        name="flexRadioDefault"
                        id="radioDefault02" 
                        checked={formik.values.gender===false}
                        onChange={() => formik.setFieldValue('gender', false)}
                    />
                    <label
                      className="mt-px inline-block pl-[0.15rem] hover:cursor-pointer text-xs font-small leading-6 text-gray-100 uppercase"
                      htmlFor="radioDefault02"
                    >
                      Female
                    </label>
                  </div>
                </div>
                <button
                  className="mt-10 transition ease-in-out duration-300 uppercase flex w-full lg:w-auto justify-center rounded-md bg-primary border-2 px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-secondary hover:text-primary focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
                  disabled={formik.isSubmitting}
                  type="submit"
                >
                  Sumbit
                </button>
              </div>

              <div className="flex flex-col">
                <div className="flex flex-col mx-[10rem] my-2">
                  <span className="font-semibold text-md">Name</span>
                  <p className="bg-white bg-opacity-10 p-2 my-1 text-gray-200 rounded-md">
                    {dateObject ? (
                      <div>{dateObject.toLocaleDateString()}</div>
                    ) : (
                      <div>invalid data</div>
                    )}
                  </p>
                  <div className="flex flex-col my-2">
                    <span className="font-semibold text-md">Last Name</span>
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
                  to={`/admin/panel/users/edit/${currentUser?.id}`}
                >
                  Assign role
                </Link>
                </div>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  );
};

export default UserEdit;
