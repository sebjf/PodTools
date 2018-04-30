using System;
using System.IO;
using Syroot.BinaryData;
using Syroot.Pod.Core;

namespace Syroot.Pod
{
    /// <summary>
    /// Represents a vehicle information block stored in a <see cref="VehiclesScript"/> for each installed car.
    /// </summary>
    public class VehicleInfo
    {
        // ---- CONSTANTS ----------------------------------------------------------------------------------------------

        private const int _key = 0x68;

        // ---- CONSTANTS ----------------------------------------------------------------------------------------------
        
        private const int _maxDisplayNameLength = 64;
        private const int _maxNameLength = 20;
        private const int _maxImageFile1Length = 13;
        private const int _maxImageFile2Length = 13;
        private const int _maxImageFile3Length = 14;

        // ---- FIELDS -------------------------------------------------------------------------------------------------

        private string _name;
        private string _menuName;
        private string _imageFile1;
        private string _imageFile2;
        private string _imageFile3;

        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        internal VehicleInfo(Stream stream)
        {
            XorByteStreamLayer xorStream = new XorByteStreamLayer(stream, _key);
            DisplayName = xorStream.ReadFixedString(_maxDisplayNameLength);
            Name = xorStream.ReadFixedString(_maxNameLength);
            ImageFile1 = xorStream.ReadFixedString(_maxImageFile1Length);
            ImageFile2 = xorStream.ReadFixedString(_maxImageFile2Length);
            ImageFile3 = xorStream.ReadFixedString(_maxImageFile3Length);
            uint runtime = xorStream.ReadUInt32();
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the case-insensitive name how the car appears in the menu. Must not be longer than 63
        /// characters.
        /// </summary>
        public string DisplayName
        {
            get { return _menuName; }
            set
            {
                // Requires 1 byte for 0-termination.
                if (value.Length > _maxDisplayNameLength - 1)
                    throw new ArgumentException($"{nameof(DisplayName)} must not exceed {_maxDisplayNameLength - 1} characters.");
                _menuName = value;
            }
        }

        /// <summary>
        /// Gets or sets the base name of the file associated with the vehicle. Must not be longer than 19 characters.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (value.Length > _maxNameLength)
                    throw new ArgumentException($"{nameof(Name)} must not exceed {_maxNameLength} characters.");
                _name = value;
            }
        }

        public string ImageFile1
        {
            get { return _imageFile1; }
            set
            {
                if (value.Length > _maxImageFile1Length)
                    throw new ArgumentException($"{nameof(ImageFile1)} must not exceed {_maxImageFile1Length} characters.");
                _imageFile1 = value;
            }
        }

        public string ImageFile2
        {
            get { return _imageFile2; }
            set
            {
                if (value.Length > _maxImageFile2Length)
                    throw new ArgumentException($"{nameof(ImageFile2)} must not exceed {_maxImageFile2Length} characters.");
                _imageFile2 = value;
            }
        }

        public string ImageFile3
        {
            get { return _imageFile3; }
            set
            {
                if (value.Length > _maxImageFile3Length)
                    throw new ArgumentException($"{nameof(ImageFile3)} must not exceed {_maxImageFile3Length} characters.");
                _imageFile3 = value;
            }
        }
        
        // ---- METHODS (INTERNAL) -------------------------------------------------------------------------------------

        internal byte[] ToBytes()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XorByteStreamLayer xorStream = new XorByteStreamLayer(stream, _key);
                xorStream.WriteFixedString(DisplayName, _maxDisplayNameLength);
                xorStream.WriteFixedString(Name, _maxNameLength);
                xorStream.WriteFixedString(ImageFile1, _maxImageFile1Length);
                xorStream.WriteFixedString(ImageFile2, _maxImageFile2Length);
                xorStream.WriteFixedString(ImageFile3, _maxImageFile3Length);
                xorStream.Write(0);
                return stream.ToArray();
            }
        }
    }
}
