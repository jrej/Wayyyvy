from PIL import Image
import os

def resize_images(input_folder, output_folder, new_width,new_height):
    # Create output folder if it doesn't exist
    if not os.path.exists(output_folder):
        os.makedirs(output_folder)

    # List all files in the input folder
    files = os.listdir(input_folder)
    images = [f for f in files if f.lower().endswith('.png')]

    for image_name in images:
        try:
            # Open each image
            image_path = os.path.join(input_folder, image_name)
            img = Image.open(image_path)

            # Get original dimensions
            width, height = img.size


            # Resize the image
            resized_img = img.resize((new_width, new_height), Image.LANCZOS)

            # Save the resized image
            output_path = os.path.join(output_folder, image_name)
            resized_img.save(output_path)
            
            print(f"Resized {image_name} from ({width}, {height}) to ({new_width}, {new_height})")

        except Exception as e:
            print(f"Error processing {image_name}: {str(e)}")

if __name__ == "__main__":
    # Input and output folder paths
    input_folder = "C:/Users/grego/My project (1)/Assets/TurnBattleSystem/Textures/Weapons2"  # Replace with your input folder containing PNG images
    output_folder = "C:/Users/grego/My project (1)/Assets/TurnBattleSystem/Textures/Weapons"  # Replace with desired output folder path

    # Resize images in the input folder and save to the output folder
    resize_images(input_folder, output_folder, 65,75)

    # Input and output folder paths
    input_folder = "C:/Users/grego/My project (1)/Assets/TurnBattleSystem/Textures/Bodies2"  # Replace with your input folder containing PNG images
    output_folder = "C:/Users/grego/My project (1)/Assets/TurnBattleSystem/Textures/Bodies"  # Replace with desired output folder path

    # Resize images in the input folder and save to the output folder
    resize_images(input_folder, output_folder, 60,100)
